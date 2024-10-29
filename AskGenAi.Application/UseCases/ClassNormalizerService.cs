using AskGenAi.Core.Entities;
using AskGenAi.Core.Interfaces;

namespace AskGenAi.Application.UseCases;

// </inheritdoc>
public class ClassNormalizerService(
    IRepository<Question> questionRepository,
    IFilePath filePath,
    IJsonFileSerializer<Discipline> disciplineFileSerializer,
    IJsonFileSerializer<Question> questionFileSerializer) : IClassNormalizerService
{
    // </inheritdoc>
    public async Task NormalizeDisciplineAsync()
    {
        var normalizeEntities =
            await disciplineFileSerializer.DeserializeAsync(filePath.GetLocalDisciplinePath());

        if (normalizeEntities is null || normalizeEntities.Data.Count == 0)
        {
            Console.WriteLine("No" + nameof(Discipline) + "classes found");
            return;
        }

        // normalize all disciplines by adding id if it is empty
        foreach (var entity in normalizeEntities.Data)
        {
            Normalize(entity);
        }

        var (fullVersion, newVersion) = IncrementVersion(normalizeEntities.Version);
        normalizeEntities.Version = fullVersion;

        var newDisciplinePath = filePath.GetLocalNewDisciplinePath(newVersion);
        var serializedEntities =
            await disciplineFileSerializer.SerializeAsync(normalizeEntities, newDisciplinePath);

        Console.WriteLine("Saved content :\n" + serializedEntities + "\nFile Path: " + newDisciplinePath);
    }

    // </inheritdoc>
    public async Task NormalizeQuestionAsync()
    {
        var normalizeEntities = await questionFileSerializer.DeserializeAsync(filePath.GetLocalQuestionsPath());

        if (normalizeEntities is null || normalizeEntities.Data.Count == 0)
        {
            Console.WriteLine("No" + nameof(Question) + "classes found");
            return;
        }

        // normalize all disciplines by adding id if it is empty
        foreach (var entity in normalizeEntities.Data)
        {
            Normalize(entity);
        }

        var (fullVersion, newVersion) = IncrementVersion(normalizeEntities.Version);
        normalizeEntities.Version = fullVersion;

        var newQuestionsPath = filePath.GetLocalNewQuestionsPath(newVersion);
        var serializedEntities =
            await questionFileSerializer.SerializeAsync(normalizeEntities, newQuestionsPath);

        Console.WriteLine("Saved content :\n" + serializedEntities + "\nFile Path: " + newQuestionsPath);
    }

    // </inheritdoc>
    public async Task NormalizeQuestionsAsync()
    {
        var normalizeEntitiesFull = new Root<Question>
        {
            Data = [],
            Version = "1.0.0"
        };

        foreach (var questionsFilename in filePath.GetQuestionsListFilename())
        {
            var normalizeEntities = await NormalizeQuestionAsync(questionsFilename);

            var enumerable = normalizeEntities as Question[] ?? normalizeEntities.ToArray();
            if (enumerable.Length == 0) 
            {
                Console.WriteLine("No" + nameof(Question) + "classes found");
                continue;
            }

            normalizeEntitiesFull.Data.AddRange(enumerable);
        }

        var questionsFullPath = filePath.GetLocalQuestionsFullPath();
        var serializedEntities =
            await questionFileSerializer.SerializeAsync(normalizeEntitiesFull, questionsFullPath);

        Console.WriteLine("Saved content :\n" + serializedEntities + "\nFile Path: " + questionsFullPath);
        Console.WriteLine("Total saved items " + normalizeEntitiesFull.Data.Count);
    }

    private async Task<IEnumerable<Question>> NormalizeQuestionAsync(string questionsFilename)
    {
        var normalizeEntities =
            await questionFileSerializer.DeserializeAsync(filePath.GetLocalQuestionsPath(questionsFilename));

        if (normalizeEntities is null || normalizeEntities.Data.Count == 0)
        {
            Console.WriteLine("No" + nameof(Question) + "classes found");
            return Array.Empty<Question>();
        }

        // normalize all disciplines by adding id if it is empty
        foreach (var entity in normalizeEntities.Data)
        {
            var questions = await questionRepository.GetAllAsync();

            var existQuestion = questions.SingleOrDefault(q => q.Context == entity.Context);

            if (existQuestion != null)
            {
                NormalizeExist(entity, existQuestion.Id);
                continue;
            }

            Normalize(entity);
        }

        Console.WriteLine(questionsFilename + "normalized");
        return normalizeEntities.Data;
    }

    private static void Normalize(IEntity entity)
    {
        if (entity.Id == Guid.Empty)
        {
            entity.Id = Guid.NewGuid();
        }
    }

    private static void NormalizeExist(IEntity entity, Guid existGuid)
    {
        entity.Id = existGuid;
    }

    // Increment the version number of the file
    private static (string, string) IncrementVersion(string version)
    {
        // version is a string like "1.0.0"
        var newVersion = version.Split('.').Select(int.Parse).ToArray();
        newVersion[2]++;
        var newFullVersionString = string.Join('.', newVersion);

        return (newFullVersionString, newVersion[2].ToString());
    }
}