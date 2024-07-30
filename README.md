# [Preguntalo!] Pregunta lo que quieras!

### *Presentación*
*presentado por* **Franco Exequiel Gonzalez**

**Mayo 2023**

### :book: Tabla de contenido

[TOC]

## 👁‍🗨 Premisa

Esta iniciativa parte desde un principio basico que es el de, **¿Que hacer cuando tengo una duda sobre alguna tematica que desconozco?** Convenientemente para muchos lo mas rapido es realizar una busqueda rapida en google para encontrar la solucion. Pero muchas veces la respuesta a una pregunta con gran nivel de especificacion no es encontrada sino hasta una exhaustiva explorarcion a traves de internet para encontrar con soluciones no tan especificas. 

#### UN EJEMPLO DE LA PROBLEMÁTICA:
Supongamos que un jubilado desea realizar una transaccion monetaria a traves de su banco ya que con sus ahorros decidio comprarse un vehiculo de segunda mano. Esta persona quiere sorprender a sus familiares por lo cual decir no contarles nada de su posible compra. Aqui tenemos una situacion de una persona mayor la cual no maneja sistemas electronicos ni tampoco conoce si el lugar donde desea comprar es de confianza; entonces surge la incertidumbre de como asegurarse de despejar ambas problematicas.

#### FORMULACIÓN DE LA PROBLEMÁTICA:
Desde ya podemos identificar una necesidad en la que seguramente mas de una persona mayor ha tenido y donde han surgido las preguntas relacionadas:
:::info
1.              ¿Como se si el sitio donde quiero comprar es de confianza? 
2.              ¿Como realizar dicha accion bancaria?
3.              ¿Que persona de confianza puede brindarme informacion de ambos temas?
:::

## Introduccion

Aqui surge una idea en la cual este proyecto quiere encontrar una posible solucion. En la que se enfocara en la tercer pregunta ya que se intentara crear un vinculo de facil acceso entre esa persona que tiene la duda, y aquella que tiene la informacion exacta y la disposicion a brindar asistencia a la primera. Esto lograra que se puedan formular preguntas de distitos rubros y puedan ser respondidos a la brevedad por interesados del tema.

#### JUSTIFICACION:
Obtener una herramienta como la que se propone ayudara a aquel que desea obtener de forma rapida las soluciones a sus problematicas. Tambien gracias a el mecanismo funcional de la herramienta podra ademas interiorizarse en diferentes tematicas y participar de comunidades de las cuales este interesado.

##  ✅ Objetivo General del Proyecto
Diseñar, construir e implementar una herramienta en la cual un usuario pueda realizar consultas de cualquier tipo y otros usuarios experimentados en la tematica puedan resolver su duda.

## ✅ Objetivos Especificos del Proyecto
*     Diseñar e implementar una herramienta en forma de app para que sea de facil acceso.
*     Implementar una base de datos para poder registrarse como usuario ademas de almacenar todas las consultas.
*     Diseñar e implementar una API la cual resuelva todas las peticiones de usuarios en cuanto a busqueda de preguntas, filtro de tematicas en las preguntas y algunos apartados avanzados.
*     Formar comunidades en las diferentes posibles tematicas empleando un sistema de foros.
*     En base al desarrollo y avance de las comunidades, obtener usuarios moderadores los cuales puedan crear comunidades saludables.
*     Formar un sistema de comunicacion directa en el "asker" (el que consulta) y el "chatter" (el que responde). EJ chat, videollamada, etc/
*     Como incentivo para que el usuario continue usando la herramienta, se pretende desarrollar un sistema de recompensa  el cual le permita ganar dinero (sacado de un porcentaje de las ganacias con anuncios) en base a su ranking de puntuacion. 

## ✅ Objetivo General del Sistema

Resolver consultas de cualquier indole implementando respuestas respaldadas por conocidos en el tema ademas de formar un vinculo mas estrecho entre el asker y el chatter.

##  Limites

El limite de este sistema se inicia desde que el usuario registrado realiza una consulta hasta la cual es resuelta.
## Alcances
Los procesos que van a ser soportados dentro del sistema son
* Registrase como usuario
* Editar perfil de usuario
* Crear consultas
* Responder consultas
* Chatear con otros usuarios
* Participar en foros
* Agregar su Profesion laboral


## No Contemplado
El sistema no contempla la validacion de las respuestas de cada usuario ni las acciones que cada usuario pueda realizar en base a las respuestas que obtenga a sus preguntas.

## Posibles Tecnologias

- Net core y entity framework para API ? 
- Mysql para base de datos ? 
- Java (native) en android studio para app movil ?
- React Native para app movil ?

## Competencia

De momento se canaliza a Quora como competencia directa, aunque este sistema apunta a tener un desarrollo mas profundo en su tematica de comunidades. Askit es otra aplicacion de la cual se encontro gran similaridad.


## Listado de Requerimientos funcionales



| ID    |       Nombre del Requerimiento        |
| ----- | ------------------------------------- |
| RF001 | Registro y administracion de usuarios |
| RF003 | Edicion de perfil                     |
| RF004 | Validacion de profesion               |
| RF005 | Registro y administracion consultas   |
| RF006 | Edicion de consultas                  |
| RF007 | Filtrar consultas                     |
| RF008 | Responder consultas                   |
| RF009 | Puntuacion de consultas               |
| RF010 | Iniciar chats entre usuarios          |

## Listado de Requerimientos no funcionales

[Plantee la lista de requerimientos no funcionales teniendo en cuenta: Usabilidad, Confiabilidad, Performance, Portabilidad, Entrega, Implementación, Estándares, Éticos, Legales, Interoperabilidad].



| ID      | Nombre del requerimiento                                                                           | Tipo                          |
| ------- | -------------------------------------------------------------------------------------------------- | ----------------------------- |
| RNF0001 | Definir niveles de usuarios y asignar funcionalidades en base al nivel                             | Seguridad y administracion    |
| RNF0002 | Establecer una interfaz intuitiva y simple                                                         | Usabilidad                    |
| RNF0003 | Se opera en una metodologia cliente (APP movil) - Servidor (API)                                   | Implementacion y Portabilidad |
| RNF0004 | Almacenar los datos en una base de datos relacional MySQL                                          | Implementacion                |
| RNF0005 | Embeber a los usuarios con un sistema de Rating (Puntuacion)                                       | Usabilidad                    |
| RNF0006 | Aplicar a consultas etiquetas y tipos de consulta para poder organizarlas en diferentes categorias | Performance                   |





## Incremento inicial (Primera muestra)
A continuacion se muestra un primer bosquejo del sistema a implementar:
### Casos de Uso especificos
Como primera instancia el sistema permitira realizar las siguientes acciones:

![](https://hackmd.io/_uploads/rkGmLOZL3.png)


### Análisis y Diseño
Para poder realizar las distintas gestiones se implementaran las siguientes clases:

**Administrador:** Int id, String nombre, String apellido, String email, String password, String fotoPerfil.

**Usuario:** Int id, String nombre, String apellido, String email, String password, String fotoPerfil, Rating rating, Validacion validacion;

**Consulta:**  Int id, Usuario usuario, String titulo, String texto, Categoria categoria, Boolean resuelto, Int respuestaSeleccionada, Int puntuacionPositiva, Int puntuacionNegativa. 

**Categoria: ** Int id, String Nombre.

**Respuesta:** Int id, Usuario usuario, String texto, Int puntuacionPositiva, Int puntuacionNegativa, Consulta consulta, Respuesta respuesta.

**Rating:** Int id, Int puntuacionPositiva, Int puntuacionNegativa, Int respuestasElegidas, Int respuestasContestadas, Int consultasRealizadas.

**Validacion:** Int id, String titulo, String entidadOtorgante, Boolean confirmada, String descripcion, List<DocumentoValidacion> documentos.

**DocumentoValidacion:** String id, String rutaDocumento.

### Diagrama Entidad-Relacion de Base de Datos
    
![](https://hackmd.io/_uploads/SyxEC6DI2.png)




    
### Diagrama de Clases
    
> en proceso... [name=Franco Gonzalez]
    
    
    
### Interfaces graficas
    
Se muestran las interfaces graficas en el siguiente enlace:
    
https://www.figma.com/file/0mm3Q2Jl2qhO9PHRdGShcC/Preguntalo!%3A-GUI?type=design&node-id=0%3A1&t=99theLaTxPrbRrIw-1
    


## Repositorio

[Enlace al repositorio con la App Móvil]
    https://github.com/kureplastic/Preguntalo 

[Enlace al repositorio con la Web API]
    https://github.com/kureplastic/preguntaloAPI
    
    
    
