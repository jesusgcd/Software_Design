﻿using EFood.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.AccesoDatos.Repositorio.IRepositorio
{
    public interface IMetodoPagoRepositorio : IRepositorio<MetodoPago>
    {
        void Actualizar(MetodoPago metodoPago);

    }
}
