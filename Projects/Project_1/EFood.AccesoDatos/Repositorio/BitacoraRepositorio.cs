using EFood.AccesoDatos.Data;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.AccesoDatos.Repositorio
{
    public class BitacoraRepositorio : Repositorio<Bitacora>, IBitacoraRepositorio
    {

        private readonly ApplicationDbContext _db;

        public BitacoraRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<Bitacora> ObtenerBitacoras()
        {
            return _db.Bitacora;
        }

    }
}