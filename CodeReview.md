# PR Review

Request changes before merge.

## Summary
This file has several compile-time problems and logic issues that should be addressed before it is safe to merge. The issues are not purely stylistic: there are invalid statements, unreachable logic, unsafe null handling, and a catch block that hides the original exception.

---

## Review comments

1. `using System.Collegctions.Generic;`
   - This namespace is misspelled. It should be `System.Collections.Generic`.
   - This is a direct compile blocker in [CodeToReview.cs](CodeToReview.cs).

2. `if ((p.Name.Length + lastName).Length > 255)`
   - This does not compile. `p.Name.Length + lastName` is not a `string`, so it has no `.Length` property.
   - The intended check likely needs to be based on the concatenated full name, e.g.:
     - `(p.Name + " " + lastName).Length > 255`

3. `(p.Name + " " + lastName).Substring(0, 255);`
   - This expression is evaluated and discarded.
   - If this is meant to enforce a max length, it should be used in a conditional or assigned to a variable. As written, it does nothing.

4. `if (random.Next(0, 1) == 0)`
   - `Random.Next(0, 1)` can only ever return `0`.
   - This means the `if` branch is always true and the `else` branch is unreachable.
   - This logic will never generate `"Betty"`.

5. `var random = new Random();`
   - This instance is created inside the loop, which is a poor pattern.
   - Reusing a single `Random` instance would be more appropriate and avoids repeated sequences from being generated too quickly.

6. `x.DOB >= DateTime.Now.Subtract(...)`
   - `x.DOB` is `DateTimeOffset`, but the comparison is against `DateTime`.
   - This mixes two different date/time types and can lead to subtle bugs.
   - It would be cleaner to use a single consistent type throughout.

7. `if (lastName.Contains("test"))`
   - This will throw a `NullReferenceException` if `lastName` is null.
   - There is no guard for invalid input.
   - A null check should be added before calling `Contains`, and case-insensitive comparison may be appropriate depending on requirements.

8. `catch (Exception e) { throw new Exception("Something failed in user creation"); }`
   - This discards the original exception and loses the stack trace.
   - Catching `Exception` broadly is also generally discouraged here.
   - If the intent is to add context, use the original exception as the inner exception or allow it to propagate.

9. Naming and design
   - `BirthingUnit` is vague and unclear.
   - `People` is awkward as a class name; `Person` would be more idiomatic.
   - `GetPeoples` is not ideal naming and is inconsistent with the rest of the code.
   - `GetMarried` does not clearly communicate what the method is actually doing.

10. `private static readonly DateTimeOffset Under16 = DateTimeOffset.UtcNow.AddYears(-15);`
   - This is static state captured once at type initialization time.
   - It looks more like a fixed cutoff value than a meaningful domain rule, and it may become stale over time.
   - The intent is unclear.

---

## Final verdict
Request changes. This file needs corrective fixes before merge due to compile errors, invalid logic, and unsafe null-handling.
