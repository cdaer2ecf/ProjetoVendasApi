using System;
using Ambev.DeveloperEvaluation.ORM;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Integration.Db;

public static class ContextFactory
{
    public static DefaultContext New()
    {
        var opts = new DbContextOptionsBuilder<DefaultContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) 
            .Options;

        return new DefaultContext(opts);
    }
}
