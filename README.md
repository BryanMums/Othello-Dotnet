# Othello
Réalisation du jeux "Othello" (Reversi) en C# avec du WPF.

## Idées de base
Réalisation d'une classe Boardgame et d'une classe Case.

### Comment détecter les coups jouables ?
* Pour chaque case vide :
  * Pour chaque voisin V8 :
    * Regarder si la case direct est de la couleur inverse, si oui continuer jusqu'à voir s'il y a une case de notre couleur, si c'est le cas, elle est jouable.
