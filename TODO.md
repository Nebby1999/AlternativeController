# Notas Generales:
1. ~~Sistema de estados ocupa Enum hardcoding (Reemplazar por sistema de estado con serializacion (Sacado de tesis))~~
	* Reescribir estados para ocupar los nuevos sistemas
2. ~~Resources Hardcoded (Black y Red) (reemplazar por Scriptable Objects que contengan metadata como nombre y color).~~
	~~* Reescribir codigo para ocupar los nuevos sistemas~~
3. Migrar a Input System
	* Un input asset
	* Player map encapsulando inputs de "Box Inputs"
  		* Player switches
   		* Cables
4. Bullets usa projectiles/game objects
	* Discutir si seguir asi o mejor usar raycast para mejor sensacion del jugador
	* Si los enemigos disparan, probablemente que sean projectiles y no raycasts.
5. Destruir sistema de "commands"
	* Commands utiliza Async/Await, el cual no es facil de usar en unity 2022
 	* Reemplazar por uso de Corutinas
# Notas Vehicle
1. Los inputs del jugador estan conectados directamente con el vehiculo (Talvez hacer un decoupling entre Entidades "vivas" y inputs (Sistema de Cuerpos y Maestros)
	* Crear un InputBank para almacenar inputs de entidades, y los componentes de estas entidades leen directamente del Input Bank
2. 
