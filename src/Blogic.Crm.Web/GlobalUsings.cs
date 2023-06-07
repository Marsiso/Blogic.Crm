// Global using directives

global using System.Globalization;
global using System.Net.Mime;
global using Blogic.Crm.Domain.Data.Dtos;
global using Blogic.Crm.Domain.Data.Entities;
global using Blogic.Crm.Domain.Routing;
global using Blogic.Crm.Infrastructure.Commands;
global using Blogic.Crm.Infrastructure.Pagination;
global using Blogic.Crm.Infrastructure.Queries;
global using Blogic.Crm.Infrastructure.TypeExtensions;
global using Blogic.Crm.Web.Views.Contract;
global using CsvHelper;
global using Mapster;
global using MediatR;
global using Microsoft.AspNetCore.Mvc;
global using Serilog;
global using static Blogic.Crm.Infrastructure.Pagination.QueryStringBase;
global using ValidationException = Blogic.Crm.Domain.Exceptions.ValidationException;