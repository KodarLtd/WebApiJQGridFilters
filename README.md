WebApiJQGridFilters
===================

Parser for jqGrid filters for ASP.NET Web API

The project contains two components to help using [jqGrid](http://www.trirand.com/blog/) filters and sorting with ASP.NET Web API.

1. A TypeConverter that converts the jqGrid JSON string to a statically typed .NET objects.
2. Several helper methods to apply filters and sorting to an IQueryable object.

The helpers use reflection to build a LINQ expression tree from the corresponding property names in the filters. The user can override the name with which the properties are exposed to the API or exclude a property from the filters. Simple relationships are also supported.
