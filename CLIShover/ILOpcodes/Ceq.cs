namespace CLIShover.ILOpCodes
{
    public class Ceq_Emitter : Interfaces.IEmitter
    {
        public void Emit(ILInstruction instr, EmitterContext ctx)
        {
            ctx.WriteLine("pop rbx");            // second operand
            ctx.WriteLine("pop rax");            // first operand
            ctx.WriteLine("cmp rax, rbx");       // compare values
            ctx.WriteLine("sete al");            // set AL = 1 if equal
            ctx.WriteLine("movzx rax, al");      // zero-extend to full register
            ctx.WriteLine("push rax");           // push result
        }
    }
}
