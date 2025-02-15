﻿using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using System.Linq;
using VokimiShared.src;
using VokimiShared.src.models.db_entities_ids;

namespace Vokimi.Helpers
{
    public class TestTakingHelper
    {
        private static string CookieName(Guid testId, Guid questionId) => $"test-taking|{testId}|{questionId}";
        public async static Task<Err> SaveSelectedAnswersToCookie(IJSRuntime JSRuntime,
                                                       Guid testId,
                                                       Guid questionId,
                                                       IEnumerable<GenericTestAnswerId> selectedAnswers) {

            var cookieName = CookieName(testId, questionId);
            var cookieValue = $"{questionId}|{string.Join("!", selectedAnswers)}";

            try {
                await JSRuntime.InvokeVoidAsync("setCookie", cookieName, cookieValue, 7);
            } catch { return new Err("Unable to save answers"); }
            return Err.None;
        }

        public static HashSet<GenericTestAnswerId> LoadSelectedAnswersFromCookie(HttpContext http,
                                                                                 Guid testId,
                                                                                 Guid questionId) {
            var cookieName = CookieName(testId, questionId);

            try {
                if (http.Request.Cookies.TryGetValue(cookieName, out var cookieValue)) {
                    var parts = cookieValue.Split('|');
                    if (parts.Length == 2 && Guid.TryParse(parts[0], out var parsedQuestionId) && parsedQuestionId == questionId) {
                        var answerIds = parts[1].Split('!', StringSplitOptions.RemoveEmptyEntries);
                        return new HashSet<GenericTestAnswerId>(answerIds.Select(id => new GenericTestAnswerId(new Guid(id))));
                    }
                }
            } catch {
                return [];
            }
            return [];
        }
    }
}
