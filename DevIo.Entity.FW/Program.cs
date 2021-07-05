using DevIo.Entity.FW.Data;
using DevIo.Entity.FW.Domain;
using DevIo.Entity.FW.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevIo.Entity.FW
{
    class Program : ProgramBase
    {
        static void Main(string[] args)
        {
           // var db = new ApplicationContext();
           // db.Database.Migrate();
          //  var existe = db.Database.GetPendingMigrations().Any(); // se existe
            Console.WriteLine("Testes");
            Console.WriteLine("Hello World!");
            // InserirDados();
            // InserirDadosEmMassa();
            // ConsultarDados();
            // CadastrarPedido();
            //ConsultarPedidoCarregamentoAdiantado();
            AtualizarDados();
            Console.ReadLine();
        }

        static void InserirDados()
        {
            var produto = new Produto
            {
                Descricao = "Produto teste",
                CodigoBarras = "123456789123",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            using var db = new Data.ApplicationContext();
           //db.Produtos.Add(produto);
           //db.Set<Produto>().Add(produto); //pode ser passado de forma generica
           //db.Entry(produto).State = EntityState.Added;
            db.Add(produto);       

            var registros = db.SaveChanges();
            Console.WriteLine(registros);

        }

        static void InserirDadosEmMassa()
        {
            var produto = new Produto
            {
                Descricao = "Produto teste",
                CodigoBarras = "123456789123",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            var cliente = new Cliente
            {
                Nome = "Marco Antonio Goncalves Junior",
                CEP = "03693010",
                Cidade = "São Paulo",
                Estado = "SP",
                Telefone = "991978461"
            };

            using var db = new Data.ApplicationContext();
            db.AddRange(produto, cliente);

            var registros = db.SaveChanges();
            Console.WriteLine(registros);

        }

        private static void ConsultarPedidoCarregamentoAdiantado()
        {
            using var db = new Data.ApplicationContext();
            var pedidos = db.Pedidos
                .Include(p => p.Itens)
                .ThenInclude(p => p.Produto)
                .ToList();

            foreach(var pedido in pedidos)
            {
                Console.WriteLine(pedido.Cliente.Nome);
            }

            Console.WriteLine(pedidos.Count);
        }

        private static void ConsultarDados()
        {
            using var db = new Data.ApplicationContext();
           // var consultaPorSintaxe = (from c in db.Clientes where c.Id > 0 select c).ToList();
            var consultaPorMetodo = db.Clientes.AsNoTracking().Where(c => c.Id > 0).ToList();

            foreach(var cliente in consultaPorMetodo)
            {
                Console.WriteLine($"Consultando clientes :  {cliente.Id}");
                //db.Clientes.Find(cliente.Id);
                db.Clientes.FirstOrDefault(p => p.Id == cliente.Id);
            }
        }
       
        private static void CadastrarPedido()
        {
            using var db = new Data.ApplicationContext();

            var cliente = db.Clientes.FirstOrDefault();
            var produto = db.Produtos.FirstOrDefault();

            var pedido = new Pedido
            {
                ClienteId = cliente.Id,
                IniciadoEm = DateTime.Now,
                FinalizadoEm = DateTime.Now,
                Observacao = "Pedido Teste",
                StatusPedido = StatusPedido.Analise,
                TipoFrete = TipoFrete.SemFrete,
                Itens = new List<PedidoItem>
                {
                    new PedidoItem
                    {
                        ProdutoId = produto.Id,
                        Desconto = 0,
                        Quantidade = 1,
                        Valor = 10
                    }
                }

            };
            db.Pedidos.Add(pedido);
            db.SaveChanges();    
        }

        public static void AtualizarDados()
        {
            using var db = new Data.ApplicationContext();

            var cliente = db.Clientes.Find(1);
            
            cliente.Nome = "Cliente alterado";


            var clienteDesconectador = new
            {
                Nome = "cliente Desconectado",
                Telefone = "123123"
            };

            db.Entry(cliente).CurrentValues.SetValues(clienteDesconectador);

            //Forçando o rastreamento , de forma explicita para atualizar todas as propriedades do cliente.
            //db.Entry(cliente).State = EntityState.Modified;
            //db.Clientes.Update(cliente);
            db.SaveChanges();
                
        }

        public static void ExcluirRegistros()
        {
            using var db = new Data.ApplicationContext();

            var clientes = db.Clientes.Find(1);

            db.Remove(clientes);
            db.SaveChanges();

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
            MapearPropriedades(modelBuilder);
        }
        public static void MapearPropriedades(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entity.GetProperties().Where(p => p.ClrType == typeof(string));

                foreach (var property in properties)
                {
                    if(string.IsNullOrEmpty(property.GetColumnType()) && !property.GetMaxLength().HasValue)
                    {
                        property.SetMaxLength(100);
                        property.SetColumnType("VARCHAR(100)");
                    }
                }
            }

        }
    }
}
