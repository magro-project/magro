using Magro.Ikura;
using Magro.Syake;
using System;
using System.Collections.Generic;

namespace Magro.Ir
{
    internal class IrConverter
    {
        // from syake

        public IrModuleDeclaration ConvertModule(SyModuleDeclaration module)
        {
            var statements = new List<IrStatement>();
            foreach (var statement in module.Statements)
            {
                statements.Add(ConvertStatement(statement));
            }

            return new IrModuleDeclaration()
            {
                Name = module.Name,
                Statements = statements,
            };
        }

        public IrStatement ConvertStatement(SyStatement statement)
        {
            throw new NotImplementedException();
        }

        public IrExpression ConvertExpression(SyExpression expression)
        {
            throw new NotImplementedException();
        }

        // from ikura

        public IrModuleDeclaration ConvertModule(IkModuleDeclaration module)
        {
            var statements = new List<IrStatement>();
            foreach (var statement in module.Statements)
            {
                statements.Add(ConvertStatement(statement));
            }

            return new IrModuleDeclaration()
            {
                Name = module.Name,
                Statements = statements,
            };
        }

        public IrStatement ConvertStatement(IkStatement statement)
        {
            throw new NotImplementedException();
        }
    }
}
