# Notas Generales:
1. ~~Sistema de estados ocupa Enum hardcoding (Reemplazar por sistema de estado con serializacion (Sacado de tesis))~~
  * Reescribir estados para ocupar los nuevos sistemas
2. ~~Resources Hardcoded (Black y Red) (reemplazar por Scriptable Objects que contengan metadata como nombre y color).~~
  ~~* Reescribir codigo para ocupar los nuevos sistemas~~
3. Migrar a Input System
# Notas Vehicle
1. Los inputs del jugador estan conectados directamente con el vehiculo (Talvez hacer un decoupling entre Entidades "vivas" y inputs (Sistema de Cuerpos y Maestros)
	* Crear un InputBank para almacenar inputs de entidades, y los componentes de estas entidades leen directamente del Input Bank
2. 