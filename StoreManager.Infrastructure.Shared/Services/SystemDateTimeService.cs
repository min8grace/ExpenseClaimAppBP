using StoreManager.Application.Interfaces.Shared;
using System;

namespace StoreManager.Infrastructure.Shared.Services
{
    public class SystemDateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;
    }
}