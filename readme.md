Tell, don't ask 
    * Andy Hunt and Dave Thomas 
    * IEEE Software jurnal (Institute of Electrical and Electronic Engineers)
    * Jan/Feb 2003
    
    We explicitly do not want 
        to query an object about its state, 
        make a decision, 
        and then tell the object what to do.
    We should not make decisions based on the called object’s
        state and then change the object’s state.

    OOP code
        send a command to an entity to get something done
        method invocation is viewed as messages being between objects 
            not as function calls (Smalltalk)

https://martinfowler.com/bliki/TellDontAsk.html
https://media.pragprog.com/articles/jan_03_enbug.pdf 
https://en.wikipedia.org/wiki/Law_of_Demeter

What else:
   * Anemic model
   * Command pattern
   * Single responsibility
   * Expression body
   * Primary constructor
   * Dependency Inversion
        ○ Who owns the interface?
        ○ Dependency inversion, composition root

 
