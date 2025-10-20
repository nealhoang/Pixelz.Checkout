// Common .NET namespaces

// Microsoft
global using Microsoft.AspNetCore.Mvc;

// Third-party
global using Hellang.Middleware.ProblemDetails;
global using MediatR;

// Api
global using Pixelz.Api.Endpoints.Orders;

// Domain
global using Pixelz.Domain.Entities;

// Application
global using Pixelz.Application;
global using Pixelz.Application.Common.Exceptions;
global using Pixelz.Application.Features.Orders.Commands.CheckoutOrder;
global using Pixelz.Application.Features.Orders.Dtos;
global using Pixelz.Application.Features.Orders.Queries.SearchOrders;

// Infrastructure
global using Pixelz.Infrastructure;

// Shared
global using Pixelz.Shared.Results;