## Code Explanation & Syntax Breakdown Rules

### Trigger Condition
Apply the following explanation format **ONLY** when the user explicitly asks to explain, analyze, or break down code, syntax, APIs, or language features (e.g., "这是什么语法", "解释一下", "拆解这段代码"). Do NOT apply this verbose format during normal code generation or bug-fixing tasks unless requested.

### Mandatory Output Structure
When explaining code or APIs, NEVER give a single-sentence high-level summary. You MUST follow this exact 4-part structure:

1. **Method / Type Signature Prototype**
   - Provide the full C# declaration signature first (including access modifiers, `static`/`virtual`/`async` keywords, generic type parameters `<T>`, generic constraints `where`, and parameter default values).

2. **Token-by-Token Syntax Breakdown**
   - Dissect the statement by syntax tokens (keywords, operators, class/method names, generic brackets, invocation parentheses).
   - Explain the exact C# grammatical role of every symbol and keyword.

3. **Parameter & Variable Deep-Dive (Table or Bullet List)**
   - For EVERY generic parameter (`<T>`), formal parameter, and key local variable, specify:
     - **Type & Namespace**: Value type vs. reference type, and its defining namespace.
     - **Passing Semantics**: Pass-by-value, pass-by-reference (`ref`/`out`/`in`), or delegate callback (`Action`/`Func` signature requirements).
     - **Internal Role & Constraints**: How the method uses this parameter internally and any type/value constraints.
     - **Concrete Example**: A valid, runnable argument example (including both Lambda and named method forms if the parameter is a delegate).

4. **Runtime Execution Flow & Common Pitfalls**
   - Trace step-by-step what the compiler (Roslyn) and runtime (CLR / framework, e.g., Prism/WPF) do behind the scenes (e.g., reflection, boxing/unboxing, DI resolution, `INotifyPropertyChanged` / `Dispatcher` behavior).
   - List 1–2 common compiler errors (with CS error codes if applicable) or runtime bugs caused by misusing this syntax.

### Strict Prohibitions
- **No Hand-Waving**: Never use vague filler phrases such as "顾名思义", "很简单", "常规用法", or "as the name suggests".
- **No Skipping Parameters**: Every single parameter in the invoked overload must be documented.