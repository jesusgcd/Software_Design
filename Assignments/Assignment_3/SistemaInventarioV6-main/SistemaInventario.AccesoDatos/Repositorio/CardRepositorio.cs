using SistemaInventarioV6.AccesoDatos.Data;
using SistemaInventarioV6.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventarioV6.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV6.AccesoDatos.Repositorio
{
    public class CardRepositorio : Repositorio<Card>, ICardRepositorio
    {

        private readonly ApplicationDbContext _db;

        public CardRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Card card)
        {
            var cardDB = _db.Cards.FirstOrDefault(reg => reg.ID == card.ID);
            if(cardDB != null)
            {
                cardDB.Description = card.Description;
                _db.SaveChanges();
            }
        }
    }
}