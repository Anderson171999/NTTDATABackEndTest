﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MicroservicioCuentaMovimientos.Models;

public partial class MOVIMIENTO
{
    public int MovimientoId { get; set; }

    public int? CuentaId { get; set; }

    public DateTime Fecha { get; set; }

    public string TipoMovimiento { get; set; }

    public string Valor { get; set; }

    public decimal Saldo { get; set; }

    public virtual CUENTA Cuenta { get; set; }
}