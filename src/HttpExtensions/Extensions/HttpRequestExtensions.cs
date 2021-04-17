using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
        /// /// <exception cref="System.Text.Json.JsonException">When the request body is not parseable into the given model.</exception>
        public static async Task<T> ParseJsonBodyAsync<T>(this HttpRequest request,[AllowNull] AbstractValidator<T> validator = null)
        {
            using var sr = new StreamReader(request.Body);
            var parsedModel = await JsonSerializer.DeserializeAsync<T>(request.Body, SerializerOptions);
            if (validator is not null)
            {
                await validator!.ValidateAndThrowAsync(parsedModel);
            }
            
            return parsedModel!;
        }

        public static IEnumerable<int> ParseIntegerList(this HttpRequest request, string queryKey)
        {
            if (queryKey is null) throw new ArgumentNullException(nameof(queryKey));

            foreach (var item in request.Query[queryKey])
            {
                if (int.TryParse(item, out var result))
                {
                    yield return result;
                }
            }
        }

        public static IEnumerable<double> ParseDoubleList(this HttpRequest request, string queryKey)
        {
            if (queryKey is null) throw new ArgumentNullException(nameof(queryKey));

            foreach (var item in request.Query[queryKey])
            {
                if (double.TryParse(item, out var result))
                {
                    yield return result;
                }
            }
        }

        public static IEnumerable<string> ParseStringList(this HttpRequest request, string queryKey)
        {
            if (queryKey is null) throw new ArgumentNullException(nameof(queryKey));

            foreach (var item in request.Query[queryKey])
            {
                yield return item;
            }
        }

        public static PaginationData GetPaginationData(
            this HttpRequest request,
            string indexParameterName = "pageIndex",
            string sizeParameterName = "pageSize",
            int maxPageSize = 20)
        {
            if (!int.TryParse(request.Query[indexParameterName], out var pageIndex))
            {
                pageIndex = 1;
            }

            if (!int.TryParse(request.Query[sizeParameterName], out var pageSize))
            {
                pageSize = 20;
            }

            if (pageSize > maxPageSize)
            {
                pageSize = maxPageSize;
            }

            return new PaginationData(pageIndex, pageSize);
        }
    }
}