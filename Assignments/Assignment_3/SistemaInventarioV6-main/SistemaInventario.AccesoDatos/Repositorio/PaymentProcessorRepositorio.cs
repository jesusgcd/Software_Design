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
    public class PaymentProcessorRepositorio : Repositorio<PaymentProcessor>, IPaymentProcessorRepositorio
    {

        private readonly ApplicationDbContext _db;

        public PaymentProcessorRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(PaymentProcessor paymentProcessor)
        {
            var paymentProcessorDB = _db.PaymentProcessors.FirstOrDefault(reg => reg.ID == paymentProcessor.ID);
            if(paymentProcessorDB != null)
            {
                paymentProcessorDB.Code = paymentProcessor.Code;
                paymentProcessorDB.Processor = paymentProcessor.Processor;
                paymentProcessorDB.Name = paymentProcessor.Name;
                paymentProcessorDB.Type = paymentProcessor.Type;
                paymentProcessorDB.Status = paymentProcessor.Status;
                paymentProcessorDB.Verification = paymentProcessor.Verification;
                paymentProcessorDB.Method = paymentProcessor.Method;
                _db.SaveChanges();
            }
        }
    }
}