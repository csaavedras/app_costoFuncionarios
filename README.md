## app_costoFuncionarios

#Objetivos:

Determinar la cantidad de dinero que recibirá un trabajador por concepto de las horas extras trabajadas en una empresa, sabiendo que cuando las horas de trabajo semanales exceden de 40, el resto se consideran horas extras y que estas se pagan 1,5 de una hora normal cuando no exceden de 2; si las horas extras exceden de 2 se pagan las primeras 2 a 1,5 de lo que se pagan las horas normales y el resto al doble. El número de horas extras no puede exceder las 4 horas diarias.
La hora de entrada es a las 9 de la mañana y la hora de salida a las 6 de la tarde. Se considera una hora de colación.
Para determinar el mes de proceso se debe contar con un ComboBox con los meses del año y un botón “Calcular”.
El resultado debe ser mostrado en un data grid.

#Todo
1. Crear comboBox enlazado con nombre_mes (Lista de todos los meses)
2. Crear un boton par accionar y rellenar el DataGridView
3. Crear un DataGridView con las columnas RUT, NOMBRE, HORAS EXTRAS y COSTO
4. Obtener datos de los funcionarios por RUT, NOMBRE con su hora de salida y entrada
5. Mostrar en el DataGridView solo funcionarios que hicieron horas extras
6. Filtrar funcionarios en el DataGridView según selección del comboBox (Por Mes)

#Contexto

Creamos una variable llamada **sqlCredencial** que nos permite conectarnos a cualquier base de datos local, para que pueda probar la aplicación con su base de datos. 
El unico cambio que realizamos fue el siguiente:

```

WITH cte
     AS (
SELECT ROW_NUMBER() OVER(PARTITION BY a.rut
       ORDER BY a.rut) AS fila,
       a.rut, 
       a.fecha, 
       LEAD(a.fecha) OVER(PARTITION BY a.rut
       ORDER BY a.fecha) AS siguiente
         FROM dbo.registro AS a
)
SELECT c.rut,
f.nombre,
       
       CASE
           WHEN c.fecha > c.siguiente
           THEN c.fecha
           ELSE c.siguiente
       END AS [IN],
       CASE
           WHEN c.siguiente IS NULL
           THEN '19000101'
           WHEN c.fecha < c.siguiente
           THEN c.fecha
           ELSE c.siguiente
       END AS [OUT]
FROM cte AS c
Inner Join funcionarios f on f.rut = c.rut
WHERE c.fila % 2 != 0;


```
