using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Agendamentos.Shared.Dados.Database
{
    public class DAL<T> where T : class
    {
        private readonly AgendamentosContext context;

        public DAL(AgendamentosContext context)
        {
            this.context = context;
        }

        public IEnumerable<T> Listar()
        {
            return context.Set<T>().ToList();
        }
        public void Adicionar(T objeto)
        {
            context.Set<T>().Add(objeto);
            context.SaveChanges();
        }
        public void Atualizar(T objeto)
        {
            context.Set<T>().Update(objeto);
            context.SaveChanges();
        }
        public void Deletar(T objeto)
        {
            context.Set<T>().Remove(objeto);
            context.SaveChanges();
        }
        public async Task<T?> RecuperarPorAsync(Expression<Func<T, bool>> condicao)
        {
            return await context.Set<T>().FirstOrDefaultAsync(condicao);
        }
        public List<T> Listar(Func<T, bool> predicate)
        {
            return context.Set<T>().Where(predicate).ToList();
        }


        public T? RecuperarPor(Func<T, bool> condicao)
        {
            return context.Set<T>().FirstOrDefault(condicao);
        }
    }
}
