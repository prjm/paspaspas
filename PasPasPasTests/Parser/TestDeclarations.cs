using Xunit;

namespace PasPasPasTests.Parser {


    public class TestDeclarations : ParserTestBase {


        [Fact]
        public void TestConstants() {
            ParseString("program test; const x = 5; .");
            ParseString("program test; resourcestring x = 5; .");
            ParseString("program test; const x = 5; const y = 5; .");
            ParseString("program test; const [z] x = 5; .");
            ParseString("program test; const [z] [k] x = 5; .");
            ParseString("program test; const [z, k] x = 5; .");
            ParseString("program test; const [z(5)] x = 5; .");
            ParseString("program test; const x = (a: 5); .");
            ParseString("program test; const x = (a: (b: 5)); .");
            ParseString("program test; const x = (5); .");
            ParseString("program test; const x = (5, 5); .");
            ParseString("program test; const x = 5 library; .");
            ParseString("program test; const x = 5 platform; .");
            ParseString("program test; const x = 5 experimental; .");
            ParseString("program test; const x = 5 deprecated; .");
            ParseString("program test; const x = 5 deprecated 'x'; .");
            ParseString("program test; const x = 5 deprecated experimental; .");
            ParseString("program test; const x = 5 deprecated experimental platform library; .");
        }

        [Fact]
        public void TestOtherDeclarations() {
            ParseString("program test; label x; .");
            ParseString("program test; label x, x, x; .");
            ParseString("program test; var x : Pointer ; .");
            ParseString("program test; var x, y : Pointer ; .");
            ParseString("program test; var [x] x : Pointer ; .");
            ParseString("program test; var [x] x : Pointer deprecated experimental; .");
            ParseString("program test; threadvar [x] x : Pointer deprecated experimental; .");
            ParseString("program test; var x, y : Pointer = 5 ; .");
            ParseString("program test; var x, y : Pointer absolute 5 ; .");
            ParseString("program test; exports x; .");
            ParseString("program test; exports x(a : Pointer) ; .");
            ParseString("program test; exports x(a : Pointer) index 4 ; .");
            ParseString("program test; exports x(a : Pointer) index 4 name 4 ; .");
            ParseString("program test; exports x(a : Pointer) index 4 name 4 resident ; .");
        }

        [Fact]
        public void TestTypeDeclarations() {
            ParseString("program test; const x : Pointer = 5; .");
            ParseString("program test; const x : array of const = 5; .");
            ParseString("program test; const x : array of Pointer = 5; .");
            ParseString("program test; const x : array [0..5] of Pointer = 5; .");
            ParseString("program test; const x : array [0..5,0..5] of Pointer = 5; .");
            ParseString("program test; const x : array [5] of Pointer = 5; .");
            ParseString("program test; const x : type dummy = 5; .");
            ParseString("program test; const x : type dummy<Pointer,Pointer> = 5; .");
            ParseString("program test; const x : set of Pointer = 5; .");
            ParseString("program test; const x : set of Pointer = 5; .");
            ParseString("program test; const x : file = 5; .");
            ParseString("program test; const x : file of Pointer = 5; .");
            ParseString("program test; const x : file of file of Pointer = 5; .");
            ParseString("program test; const x : packed array [0..5] of Pointer = 5; .");
            ParseString("program test; const x : packed set of Pointer = 5; .");
            ParseString("program test; const x : packed file = 5; .");
            ParseString("program test; const x : string = 5; .");
            ParseString("program test; const x : string[5] = 5; .");
            ParseString("program test; const x : ansistring = 5; .");
            ParseString("program test; const x : ansistring(222) = 5; .");
            ParseString("program test; const x : shortstring = 5; .");
            ParseString("program test; const x : widestring = 5; .");
            ParseString("program test; const x : unicodestring = 5; .");
        }

        [Fact]
        public void TestClassTypeDefinitions() {
            ParseString("unit test; interface implementation procedure x.x ; begin end ; end .");
            ParseString("unit test; interface implementation function x.x : x ; begin end ; end .");
            ParseString("unit test; interface implementation class function x.x : x ; begin end ; end .");
            ParseString("unit test; interface implementation class procedure x.x ; begin end ; end .");
            ParseString("unit test; interface implementation constructor x.x ; begin end ; end .");
            ParseString("unit test; interface implementation destructor x.x ; begin end ; end .");
            ParseString("unit test; interface implementation class constructor x.x ; begin end ; end .");
            ParseString("unit test; interface implementation class destructor x.x ; begin end ; end .");
            ParseString("unit test; interface implementation [x] procedure x.x ; begin end ; end .");
            ParseString("unit test; interface implementation procedure x ; overload; begin end ; end .");
            ParseString("unit test; interface implementation procedure x ; inline; begin end ; end .");
            ParseString("unit test; interface implementation procedure x ; cdecl; pascal; register; safecall; stdcall; export; begin end ; end .");
            ParseString("unit test; interface implementation procedure x ; local; near; far; export; begin end ; end .");
            ParseString("unit test; interface implementation procedure x ; varargs; begin end ; end .");
            ParseString("unit test; interface implementation procedure x ; external; begin end ; end .");
            ParseString("unit test; interface implementation procedure x ; external 5 name 5 index 5; begin end ; end .");
        }

        [Fact]
        public void TestClassTypeDeclarations() {
            ParseString("program test; const x : procedure = 5; .");
            ParseString("program test; const x : procedure () = 5; .");
            ParseString("program test; const x : procedure (x : Pointer) = 5; .");
            ParseString("program test; const x : function (x : Pointer) : Pointer = 5; .");
            ParseString("program test; const x : function (x : Pointer) : [x] Pointer = 5; .");
            ParseString("program test; const x : reference to function (x : Pointer) : [x] Pointer = 5; .");
            ParseString("program test; const x : reference to procedure (x : Pointer) = 5; .");
            ParseString("program test; const x : x = 5; .");
            ParseString("program test; const x : x.x = 5; .");
            ParseString("program test; const x : type x.x = 5; .");
            ParseString("program test; const x : ( x ) = 5; .");
            ParseString("program test; const x : ( x , y ) = 5; .");
            ParseString("program test; const x : ( x = 1 , y = 1 ) = 5; .");
            ParseString("program test; const x : a = 5; .");
            ParseString("program test; const x : 5..5 = 5; .");
            ParseString("program test; type x = Pointer; .");
            ParseString("program test; type x<a> = Pointer; .");
            ParseString("program test; type x<a, b> = Pointer; .");
            ParseString("program test; type x<a, b> = Pointer; .");
            ParseString("program test; type x<a; b : constructor> = Pointer; .");
            ParseString("program test; type x<a : record; b : class; c : constructor; d : q> = Pointer; .");
            ParseString("program test; type x<a> = q<a>; .");
            ParseString("program test; const x : class of dummy = 5; .");
            ParseString("program test; const x : class of dummy.x = 5; .");
            ParseString("program test; const x : class end = 5; .");
            ParseString("program test; const x : class sealed end = 5; .");
            ParseString("program test; const x : class abstract end = 5; .");
            ParseString("program test; const x : class(x) end = 5; .");
            ParseString("program test; const x : class(x.z) end = 5; .");
            ParseString("program test; const x : class public end = 5; .");
            ParseString("program test; const x : class protected end = 5; .");
            ParseString("program test; const x : class private end = 5; .");
            ParseString("program test; const x : class published end = 5; .");
            ParseString("program test; const x : class automated end = 5; .");
            ParseString("program test; const x : class strict protected end = 5; .");
            ParseString("program test; const x : class strict private end = 5; .");
            ParseString("program test; const x : class procedure x; end = 5; .");
            ParseString("program test; const x : class constructor x; end = 5; .");
            ParseString("program test; const x : class destructor x; end = 5; .");
            ParseString("program test; const x : class class procedure x; end = 5; .");
            ParseString("program test; const x : class class constructor x; end = 5; .");
            ParseString("program test; const x : class class destructor x; end = 5; .");
            ParseString("program test; const x : class procedure x<x>; end = 5; .");
            ParseString("program test; const x : class procedure x<x, x>; end = 5; .");
            ParseString("program test; const x : class procedure x<x : class; z>; end = 5; .");
            ParseString("program test; const x : class procedure x<x : record; z>; end = 5; .");
            ParseString("program test; const x : class procedure x<x : constructor; z>; end = 5; .");
            ParseString("program test; const x : class procedure x<x : x; z>; end = 5; .");
            ParseString("program test; const x : class procedure x(x); end = 5; .");
            ParseString("program test; const x : class procedure x(x : Pointer); end = 5; .");
            ParseString("program test; const x : class procedure x([q] x : Pointer); end = 5; .");
            ParseString("program test; const x : class procedure x([q] [q] x, y : Pointer); end = 5; .");
            ParseString("program test; const x : class procedure x(const x); end = 5; .");
            ParseString("program test; const x : class procedure x(var x); end = 5; .");
            ParseString("program test; const x : class procedure x(out x); end = 5; .");
            ParseString("program test; const x : class procedure x(x = 5); end = 5; .");
            ParseString("program test; const x : class function x: Pointer; end = 5; .");
            ParseString("program test; const x : class function x(x): Pointer; end = 5; .");
            ParseString("program test; const x : class function x(x, x): Pointer; end = 5; .");
            ParseString("program test; const x : class procedure x; reintroduce; end = 5; .");
            ParseString("program test; const x : class procedure x; overload; end = 5; .");
            ParseString("program test; const x : class procedure x; message 5; end = 5; .");
            ParseString("program test; const x : class procedure x; static; end = 5; .");
            ParseString("program test; const x : class procedure x; dynamic; end = 5; .");
            ParseString("program test; const x : class procedure x; override; end = 5; .");
            ParseString("program test; const x : class procedure x; virtual; end = 5; .");
            ParseString("program test; const x : class procedure x; final; end = 5; .");
            ParseString("program test; const x : class procedure x; inline; end = 5; .");
            ParseString("program test; const x : class procedure x; assembler; end = 5; .");
            ParseString("program test; const x : class procedure x; cdecl; end = 5; .");
            ParseString("program test; const x : class procedure x; pascal; end = 5; .");
            ParseString("program test; const x : class procedure x; register; end = 5; .");
            ParseString("program test; const x : class procedure x; safecall; end = 5; .");
            ParseString("program test; const x : class procedure x; stdcall; end = 5; .");
            ParseString("program test; const x : class procedure x; export; end = 5; .");
            ParseString("program test; const x : class procedure x = t; end = 5; .");
            ParseString("program test; const x : class procedure x.x = t; end = 5; .");
            ParseString("program test; const x : class const q = 5; z = 3; end = 5; .");
            ParseString("program test; const x : class type x = class end; end = 5; .");
            ParseString("program test; const x : class property x; end = 5; .");
            ParseString("program test; const x : class [x] property x; end = 5; .");
            ParseString("program test; const x : class [x] class property x; end = 5; .");
            ParseString("program test; const x : class [x] class property x [ a : Pointer ] ; end = 5; .");
            ParseString("program test; const x : class [x] class property x [ a : Pointer ] : x.y ; end = 5; .");
            ParseString("program test; const x : class [x] class property x [ a : Pointer ] : x.y index 5 ; end = 5; .");
            ParseString("program test; const x : class [x] class property x [ a : Pointer ] : x.y index 5 read a; end = 5; .");
            ParseString("program test; type x = class procedure a.b.c.d = x; end; .");
            ParseString("program test; type x = class helper for x end; .");
            ParseString("program test; type x = class helper for x procedure x; end; .");
            ParseString("program test; type x = class helper for x [x] class procedure x; end; .");
            ParseString("program test; type x = class helper for x [x] function x: x; end; .");
            ParseString("program test; type x = class helper (x) for x [x] function x: x; end; .");
            ParseString("program test; type x = class helper (x, y) for x [x] function x: x; end; .");
            //ParseString("program test; type x = interface; .");
            ParseString("program test; type x = interface (x) end; .");
            ParseString("program test; type x = interface (x) ['x'] end; .");
            ParseString("program test; type x = interface (x) ['x'] function x: x; end; .");
            ParseString("program test; type x = interface (x) ['x'] procedure x; end; .");
            ParseString("program test; type x = interface (x) ['x'] property x : Pointer read x write x; end; .");
            ParseString("program test; type x = object end; .");
            ParseString("program test; type x = object (x) end; .");
            ParseString("program test; type x = object (x) public function x: x; end; .");
            ParseString("program test; type x = record end; .");
            ParseString("program test; type x = record x, x: x ; end; .");
            ParseString("program test; type x = record x, x: x ; case Pointer of 5 : ( x: x ; )  end; .");
            ParseString("program test; type x = record helper for x end ; .");
            ParseString("program test; type x = record helper for x function x: x; end ; .");
            ParseString("program test; type x = record helper for x property x : x read x; end ; .");
            ParseString("program test; [assembly:x.y.z(5)] .");
        }


    }
}
