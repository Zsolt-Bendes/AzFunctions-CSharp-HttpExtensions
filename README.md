# Http extensions methods for Azure Functions C#
[![.NET](https://github.com/Zsolt-Bendes/AzFunctions-CSharp-HttpExtensions/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Zsolt-Bendes/AzFunctions-CSharp-HttpExtensions/actions/workflows/dotnet.yml)
## Authorization Extension
`string GetAuthorization(this HttpRequest req, string authenticationType)`

Returns the Upn claim from `HttpRequest`.
## HttpRequest Extensions
`Task<T> ParseJsonBodyAsync<T>(this HttpRequest request, AbstractValidator<T> validator)`

Parses the request json into the specified model. For validation the method uses FluentValidator.

`IEnumerable<int> ParseIntegerList(this HttpRequest request, string queryKey)`

Returns the list of `integer`s from query parameter.

`IEnumerable<double> ParseDoubleList(this HttpRequest request, string queryKey)`

Returns the list of `double` from query parameter.

`IEnumerable<string> ParseStringList(this HttpRequest request, string queryKey)`

Returns the list of `string`s from query parameter.

`PaginationData GetPaginationData(
            this HttpRequest request,
            string indexParameterName = "pageIndex",
            string sizeParameterName = "pageSize",
            int maxPageSize = 20)`

Parses a paginitation request model from query string.