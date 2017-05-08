# Parsing notes

I've found some minor parsing issues and try to 
document them here briefly.

#### Semicolon can be left out before an `overload` directive:

Example:

The following declaration

    constructor Create(instanceClass: TClass) overload;

is allowed and parsed valid.

####  The `()`  construct is ambigious

`()`  it can be a empty record consant value
_or_  an empty set  declaraton. This depends on context.

