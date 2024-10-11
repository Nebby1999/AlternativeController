##### Este es un documento en MarkDown, utilize VisualStudioCode o StackEdit.IO para visualizar

# Layer Index

La Clase LayerIndex es simplemente una clase generada automaticamente por el proyecto

Contiene simplemente los Layers de GameObjects usados para fisicas y demases, Es mas facil usar esto para referenciar las layers del proyecto enves de usar strings, dado que los strings son propensos a ser escritos mal.

Cada opcion es una instancia de LayerIndex, el cual contiene informacion del layer en si.

La clase y su confiugracion de creacion se encuentra en el Project settings, se puede conseguir usando el toolbar y seleccionando "Edit->ProjectSettings->Nebula"

## Propiedades de instancia de un LayerIndex

* IntVal: Es el valor de Integer de la layer en si. Con este valor por ejemplo se puede cambiar el layer de un GameObject
* mask: Es la Mascara de Integer usado para colisiones, ocupar el valor "Mask" retorna directamente el struct de tipo "LayerMask" de unity en si.
* collissionMask: Es la mascara de colision, se puede usar para saber si un layer tiene colision con otro layer.

## Clase CommonMasks

Es una clase estatica la cual tiene LayerMasks de tipo read only, estas mascaras son basicamente combinaciones de mascaras usadas por el juego, por ejemplo, los "HitscanAttacks" ocupan la common mask "Bullet".

### Como generar una LayerMask con multiples mascaras.

Se ocupa el "Bitwise OR" operador.

    LayerMask miMascaraCombinada = (int)LayerIndex.world.mask | (int)LayerIndex.entityPrecise;

El codigo arriba, generara una layer mask para colisionar con objetos que estan en el layer World, o en el layer EntityPrecise.

### Actualizar el struct:

