# Tank Analysis

* If both tracks are accelerated, the tank moves forward.

* If both tracks are going in reverse, the tank moves backwards.

* If one track is going forwards and the other backwards, the tank rotates in place, towards the track going backwards.
    * LF && RB == Rotate Right
    * LB && RF == Rotate Left

* If one track is moving and the other is not, the tank moves in the desired direction and rotates towards the stopped track, the rotation is reversed if the moving track is going backwards.
    * LF && RS == Move forwards and rotate right
    * LB && RS == Move backwards and rotate left
    * LS && RF == Move forwards and rotate left
    * LS && RB == Move backwards and rotate right