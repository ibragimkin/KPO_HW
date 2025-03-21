﻿using Domain.Entities;

namespace Domain.Visitor
{
    public interface IExportVisitor
    {
        void Visit(BankAccount account);
        void Visit(Category category);
        void Visit(Operation operation);
    }
}
