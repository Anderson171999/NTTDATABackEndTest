﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MicroservicioClientePersona.Models;

public partial class CLIENTE
{
    public int ClienteId { get; set; }

    public int? PersonaId { get; set; }

    public string Contraseña { get; set; }

    public bool Estado { get; set; }

    public virtual PERSONA Persona { get; set; }
}