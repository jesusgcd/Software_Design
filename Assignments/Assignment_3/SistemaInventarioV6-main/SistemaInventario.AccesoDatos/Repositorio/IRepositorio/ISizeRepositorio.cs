﻿using SistemaInventarioV6.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV6.AccesoDatos.Repositorio.IRepositorio
{
    public interface ISizeRepositorio : IRepositorio<Size>
    {
        void Actualizar(Size size);
    }
}
