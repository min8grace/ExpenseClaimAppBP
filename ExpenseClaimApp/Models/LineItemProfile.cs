using AutoMapper;
using StoreManager.Application.Features.LineItems.Commands.Create;
using StoreManager.Application.Features.LineItems.Commands.Update;
using StoreManager.Domain.Entities.Expense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseClaimApp.Models
{
    public class LineItemProfile : Profile
    {
        public LineItemProfile()
        {
            CreateMap<LineItem, LineItemEditModel>();
            CreateMap<LineItemEditModel, LineItem>();
            CreateMap<LineItem, UpdateLineItemCommand>();
            CreateMap<LineItem, CreateLineItemCommand>();
        }
    }
}
