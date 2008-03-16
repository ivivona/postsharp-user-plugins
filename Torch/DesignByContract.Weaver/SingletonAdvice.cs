﻿using System;
using System.Collections.Generic;
using PostSharp.CodeWeaver;
using PostSharp.CodeModel;
using PostSharp.Collections;

namespace Torch.DesignByContract.Weaving.Advices
{
    public class SingletonAdvice : IAdvice
    {
        private TypeDefDeclaration m_type;

        public SingletonAdvice(TypeDefDeclaration typeDef)
        {
            m_type = typeDef;
        }

        #region IAdvice Members

        public int Priority
        {
            get { return int.MinValue; }
        }

        public bool RequiresWeave(WeavingContext context)
        {
            return true;
        }

        public void Weave(WeavingContext context, InstructionBlock block)
        {
            InstructionSequence sequence = null;
            
            sequence = context.Method.MethodBody.CreateInstructionSequence();
            block.AddInstructionSequence(sequence, NodePosition.After, null);
            context.InstructionWriter.AttachInstructionSequence(sequence);
            // TODO: sacar esto
            context.WeavingHelper.WriteLine("aca {0}", context.InstructionWriter, context.InstructionReader.CurrentInstruction);
            context.WeavingHelper.WriteLine("aca {0}", context.InstructionWriter, m_type);

            context.InstructionWriter.EmitInstructionMethod(OpCodeNumber.Call, context.Method.Module.FindMethod(m_type.Methods.GetOneByName("get_Instance").GetReflectionWrapper(new Type[]{},new Type[]{}),BindingOptions.Default));
            context.InstructionWriter.DetachInstructionSequence();
            
        }

        #endregion
    }
}
