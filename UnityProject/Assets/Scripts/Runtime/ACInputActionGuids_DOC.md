##### Este es un documento en MarkDown, utilize VisualStudioCode o StackEdit.IO para visualizar

# ACInputActionGUIDs

el ACInputActionGUIDs es una clase la cual es generada automaticamente por el proyecto, contiene los GUIDS de input system usado por el InputActionAsset del juego.

Se puede ocupar para conseguir los InputActions de manera directa, dado que son constantes y no strings, los strings son faciles de escribir mal causando problemas.

El InputActionAsset y su confiugracion de creacion se encuentra en el Project settings, se puede conseguir usando el toolbar y seleccionando "Edit->ProjectSettings->Nebula"

Contiene dos mapas:

## HQMap

Contiene los inputs de los HeadQuarters, incluyendo los inputs de coneccion de cables como:

* Switch to battle state 1: Input para cambiar de estado de pelea al vehiculo 1
* Switch to battle state 2: Input para cambiar de estado de pelea al vehiculo 2
* Red Resources: Cableado usado para mandar el recurso rojo a una de las dos bases
* Black Resources: Cableado usado para mandar el recurso negro a una de las dos bases

## VehicleMap

Contiene los inputs de los Vehiculos.

* DissipateHeat: La habilidad de disipar calor esta ligado a este input.
* HarvestShoot: La habilidad de minar o disparar el laser esta ligado a este input.
* Decoy_Stun: La habilidad de soltar un decoy o defender esta ligado a este input.
* LeftTrack: El movimiento de la oruga izquierda esta ligado a este input.
* Right Track: El movimiento de la oruga derecha esta ligado a este input.