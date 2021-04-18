using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using FluentValidation;
using HttpExtensions.Models;
using Microsoft.AspNetCore.Http;

namespace HttpExtensions.Extensions
{
    public static class HttpRequestExtensions
    {
        private static readonly JsonSerializerOptions SerializerOptions = new() {PropertyNameCaseInsensitive = true};

        /// <summary>
        /// Validates the parsed request body.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="validator">The specified validator for the return type.</param>
        /// <typeparam name="T">Type of the of the model that will should be returned.</typeparam>
        /// <returns>The parsed model.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="System.Text.Json.JsonException">When the request body is not parseable into the given model.</exception>
        public static async Task<T> ParseJsonBodyAsync<T>(this HttpRequest request,
            [AllowNull] AbstractValidator<T> validator = null)
        {
            using var sr = new StreamReader(request.Body);
            var parsedModel = await JsonSerializer.DeserializeAsync<T>(request.Body, SerializerOptions);
            if (validator is not null) await validator!.ValidateAndThrowAsync(parsedModel);

            return parsedModel!;
        }

        /// <summary>
        /// Parses a list of integers from query string. The expected format is `key=1.2,1.1`.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="queryKey">The parameter name that holds the values.</param>
        /// <param name="separator">The separator char for splitting the values.</param>
        /// <returns>The list of parsed doubles.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<int> ParseIntegerList(this HttpRequest request, string queryKey,
            string separator = ",")
        {
            if (queryKey is null) throw new ArgumentNullException(nameof(queryKey));

            var values = request.Query[queryKey].ToString();
            if (string.IsNullOrEmpty(values)) yield break;

            foreach (var item in values.Split(separator))
                if (int.TryParse(item, out var result))
                    yield return result;
        }

        /// <summary>
        /// Parses a list of doubles from query string. The expected format is `key=1.2,1.1`.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="queryKey">The parameter name that holds the values.</param>
        /// <param name="separator">The separator char for splitting the values.</param>
        /// <returns>The list of parsed doubles.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<double> ParseDoubleList(this HttpRequest request, string queryKey,
            string separator = ",")
        {
            if (queryKey is null) throw new ArgumentNullException(nameof(queryKey));

            var values = request.Query[queryKey].ToString();
            if (string.IsNullOrEmpty(values)) yield break;

            foreach (var item in values.Split(separator))
                if (double.TryParse(item, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
                    yield return result;
        }

        /// <summary>
        /// Parses a list of strings from query string. The expected format is `key=str1,str2`.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="queryKey">The parameter name that holds the values.</param>
        /// <param name="separator">The separator char for splitting the values.</param>
        /// <returns>The list of parsed strings.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<string> ParseStringList(this HttpRequest request, string queryKey,
            string separator = ",")
        {
            if (queryKey is null) throw new ArgumentNullException(nameof(queryKey));

            var values = request.Query[queryKey].ToString();
            if (string.IsNullOrEmpty(values)) yield break;

            foreach (var item in values.Split(separator)) yield return item;
        }

        /// <summary>
        ///     Returns pagination model.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="indexParameterName">Name of the query parameter that holds the index value.</param>
        /// <param name="sizeParameterName">Name of the query parameter that holds the page size value.</param>
        /// <param name="maxPageSize">Size of the requested page.</param>
        /// <returns></returns>
        public static PaginationData GetPaginationData(
            this HttpRequest request,
            string indexParameterName = "pageIndex",
            string sizeParameterName = "pageSize",
            int maxPageSize = 20)
        {
            if (!int.TryParse(request.Query[indexParameterName], out var pageIndex)) pageIndex = 1;

            if (!int.TryParse(request.Query[sizeParameterName], out var pageSize)) pageSize = 20;

            if (pageSize > maxPageSize) pageSize = maxPageSize;

            return new PaginationData(pageIndex, pageSize);
        }
    }
}