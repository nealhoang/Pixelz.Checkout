// System
global using System.Reflection;

// Microsoft
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.DependencyInjection;

// Third-party
global using FluentValidation;
global using FluentValidation.Results;
global using MediatR;
global using AutoMapper;

// Domain
global using Pixelz.Domain.Entities;
global using Pixelz.Domain.Enums;

//Application
global using Pixelz.Application.Common.Behaviors;
global using Pixelz.Application.Common.Mapping;
global using Pixelz.Application.Common.Models;
global using Pixelz.Application.Common.Exceptions;
global using Pixelz.Application.Interfaces;
global using Pixelz.Application.Interfaces.Repositories;
global using Pixelz.Application.Interfaces.Services;
global using Pixelz.Application.Features.Orders.Dtos;

// Messaging
global using Pixelz.Messaging.Events;

// Shared
global using Pixelz.Shared.Results;

