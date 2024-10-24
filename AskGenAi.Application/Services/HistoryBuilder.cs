using System.Text;
using AskGenAi.Core.Interfaces;

namespace AskGenAi.Application.Services;

// </inheritdoc>
public class HistoryBuilder : IHistoryBuilder
{
    private readonly StringBuilder _stringBuilder = new();

    // </inheritdoc>
    public string BuildQuestionHistory(string? personality, string? technologies, string? discipline,
        string? subDiscipline)
    {
        if (string.IsNullOrWhiteSpace(personality) || string.IsNullOrWhiteSpace(technologies) ||
            string.IsNullOrWhiteSpace(discipline))
        {
            return string.Empty;
        }

        _stringBuilder.Clear();

        const string template = """
                                You are an experienced personalityTemplate with extensive hands-on experience working with complex software products that utilize technologies such as 
                                <technologies>technologiesTemplate</technologies> in <discipline>disciplineTemplate</discipline>. 
                                When I ask you, a question related to these technologies or other information technologies, here are the steps you should follow:
                                1. Read the question carefully
                                2. Then, think through the question and analyze all relevant considerations
                                3. After thinking and analyzing, provide your detailed answer to the question, including thought process and reasoning for your response. 
                                Your response should have such structure:
                                Thought process: [Explain your reasoning and justification for the answer, provide relevant factors you considered] 
                                Answer: [Your final, detailed answer to the question]
                                All section titles should be bold.
                                Make sure to provide a thorough and well-reasoned response, utilizing your extensive experience as a personalityTemplate working with the specified technologies. 
                                If you need to make any assumptions, state them clearly.
                                """;

        _stringBuilder
            .Append(template.Replace("personalityTemplate", personality)
                .Replace("technologiesTemplate", technologies)
                .Replace("disciplineTemplate", ConstructDiscipline(discipline, subDiscipline)));

        return _stringBuilder.ToString();
    }

    private static string ConstructDiscipline(string? discipline, string? subDiscipline)
    {
        if (string.IsNullOrWhiteSpace(subDiscipline))
        {
            return discipline + ".";
        }

        return discipline + "." + subDiscipline + ".";
    }
}
