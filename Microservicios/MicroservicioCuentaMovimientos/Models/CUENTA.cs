﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MicroservicioCuentaMovimientos.Models;

public partial class CUENTA
{
    public int CuentaId { get; set; }

    public string NumeroCuenta { get; set; }

    public int? ClienteId { get; set; }

    public string TipoCuenta { get; set; }

    public decimal SaldoInicial { get; set; }

    public bool Estado { get; set; }

    public virtual ICollection<MOVIMIENTO> MOVIMIENTO { get; set; } = new List<MOVIMIENTO>();
}