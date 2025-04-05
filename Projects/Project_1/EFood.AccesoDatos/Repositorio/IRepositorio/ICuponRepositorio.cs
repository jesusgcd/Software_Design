using EFood.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.AccesoDatos.Repositorio.IRepositorio
{
    public interface ICuponRepositorio : IRepositorio<Cupon>
    {
        void Actualizar(Cupon cupones);

    }
}
