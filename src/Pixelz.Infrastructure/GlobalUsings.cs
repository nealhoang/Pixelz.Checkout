// Common .NET namespaces

// System
global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Threading;
global using System.Threading.Tasks;
global using System.Text.Json;

// Microsoft 
global using Microsoft.Extensions.Logging;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Design;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.Extensions.Hosting;

global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;

// Third-party
global using Bogus;
global using MediatR;

// Domain
global using Pixelz.Domain.Common;
global using Pixelz.Domain.Entities;
global using Pixelz.Domain.Enums;

// Application
global using Pixelz.Application.Interfaces;
global using Pixelz.Application.Interfaces.Repositories;
global using Pixelz.Application.Interfaces.Services;
global using Pixelz.Application.Common.Models;

// Messaging
global using Pixelz.Messaging;
global using Pixelz.Messaging.Events;

// Shared
global using Pixelz.Shared.Results;

// Infrastructure
global using Pixelz.Infrastructure.Persistence.Contexts;
global using Pixelz.Infrastructure.Repositories;
global using Pixelz.Infrastructure.Outbox;
global using Pixelz.Infrastructure.Persistence;
global using Pixelz.Infrastructure.Services;
global using Pixelz.Infrastructure.Messaging;
global using Pixelz.Infrastructure.Persistence.Extensions;


