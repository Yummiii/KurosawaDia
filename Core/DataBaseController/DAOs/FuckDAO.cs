﻿using DataBaseController.Contexts;
using DataBaseController.Modelos;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace DataBaseController.DAOs {
    public sealed class FuckDAO {
        public async Task<Fuck> Get(Fuck fuck)
        {
            using (Kurosawa_DiaContext context = new Kurosawa_DiaContext())
            {
                return (await context.Fuck.FromSqlRaw("call GetFuck({0})", fuck.Explicit).ToListAsync()).FirstOrDefault();
            }
        }

        public void Add(Fuck fuck)
        { 
            using (Kurosawa_DiaContext context = new Kurosawa_DiaContext())
            {
                //IDbContextTransaction transation = await context.Database.BeginTransactionAsync(IsolationLevel.Snapshot);
                //await context.Database.ExecuteSqlRawAsync("call AddFuck({0}, {1}, {2})", fuck.Usuario.ID, fuck.Url, fuck.Explicit);
                _ = context.Fuck.FromSqlRaw("call AddFuck({0}, {1}, {2})", fuck.Usuario.ID, fuck.Url, fuck.Explicit);
                //await transation.CommitAsync();
            }
        }
    }
}
