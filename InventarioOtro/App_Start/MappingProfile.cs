using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Profile;
using AutoMapper;
using InventarioOtro.Dtos;
using InventarioOtro.Models;


namespace InventarioOtro.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<Cliente, ClienteDto>();
                

            Mapper.CreateMap<ClienteDto, Cliente>();


        }
    }
}