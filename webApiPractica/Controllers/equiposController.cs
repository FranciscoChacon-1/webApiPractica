﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webApiPractica.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace webApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class equiposController : ControllerBase
    {
        private readonly equiposContext _equiposContext;
        public equiposController(equiposContext equiposContext)
        {
            _equiposContext = equiposContext;

        }

        /// <sumary>
        /// EndPoint que retorna el listado de todos los equipos existentes
        /// ESTO ES UNA PRUEBA PARA VER SI NOS SALE LO DEL COMMIT Y ESA PARTE de pruebas
        /// ahi esta mi prueba
        /// CAMBIOS PARA HACER OTRA PRUEBA DE LA PRUEBA XD
        /// conftimando la prueba de la prubeba que hicimos de la prueba
        /// </sumary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            var listadoEquipo = (from e in _equiposContext.equipos
                                 join t in _equiposContext.tipo_equipo
                                 on e.tipo_equipo_id equals t.id_tipo_equipo
                                 join m in _equiposContext.marcas
                                 on e.marca_id equals m.id_marcas
                                 join es in _equiposContext.estados_equipo
                                 on e.estado_equipo_id equals es.id_estados_equipo
                                 select new
                                 {
                                     e.id_equipos,
                                     e.nombre,
                                     e.descripcion,
                                     e.tipo_equipo_id,
                                     tipo_equipo = t.descripcion,
                                     e.marca_id,
                                     marca = m.nombre_marca,
                                     e.estado_equipo_id,
                                     estado_equipo = es.descripcion,
                                     detalle = $"Tipo : {t.descripcion}, Marca {m.nombre_marca}, Estado equipo {es.descripcion}",
                                     e.estado
                                 }
                                           ).OrderBy(resultado=>resultado.estado_equipo_id)
                                           .ThenBy(resultado => resultado.marca_id)
                                           .ThenByDescending(resultado => resultado.tipo_equipo_id)
                                           .ToList();


            if (listadoEquipo.Count() == 0)
            {
                return NotFound();
            }
            return Ok(listadoEquipo);
        }

        /// <sumary>
        /// EndPoint que retorna el listado de todos los equipos existentes
        /// </sumary>
        /// <param name="id"></param>>
        /// <returns></returns>
        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult Get(int id)
        {
            equipos? equipo = (from e in _equiposContext.equipos
                               where e.id_equipos == id
                               select e).FirstOrDefault();

            if (equipo == null)
            {
                return NotFound();
            }
            return Ok(equipo);
        }

        /// <sumary>
        /// EndPoint que retorna el listado de todos los equipos existentes
        /// </sumary>
        /// <param name="id"></param>>
        /// <returns></returns>
        [HttpGet]
        [Route("Find/{filtro}")]
        public IActionResult FindByDescription(string filtro)
        {
            equipos? equipo = (from e in _equiposContext.equipos
                               where e.descripcion.Contains(filtro)
                               select e).FirstOrDefault();

            if (equipo == null)
            {
                return NotFound();
            }
            return Ok(equipo);
        }

        /// <sumary>
        /// EndPoint que retorna el listado de todos los equipos existentes
        /// </sumary>
        /// <param name="id"></param>>
        /// <returns></returns>
        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarEquipo([FromBody] equipos equipo)
        {
            try
            {
                _equiposContext.equipos.Add(equipo);
                _equiposContext.SaveChanges();
                return Ok(equipo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarEquipo(int id, [FromBody] equipos equipoModificar)
        {
            //Para actualizar un registro, se obtiene el registro original de la base de datos
            //al cual alteraremos alguna propiedad
            equipos? equipoActual = (from e in _equiposContext.equipos
                                     where e.id_equipos == id
                                     select e).FirstOrDefault();
            //Verificamos que exista el registro segun su ID
            if (equipoActual == null)
            { return NotFound(); }
            //Si se encuentra el registro, se alteran los campos modificables
            equipoActual.nombre = equipoModificar.nombre;
            equipoActual.descripcion = equipoModificar.descripcion;
            equipoActual.marca_id = equipoModificar.marca_id;
            equipoActual.tipo_equipo_id = equipoModificar.tipo_equipo_id;
            equipoActual.anio_compra = equipoModificar.anio_compra;
            equipoActual.costo = equipoModificar.costo;
            //Se marca el registro como modificado en el contexto //y se envia la modificacion a la base de datos
            _equiposContext.Entry(equipoActual).State = EntityState.Modified;
            _equiposContext.SaveChanges();
            return Ok(equipoModificar);

        }

        [HttpDelete]
        [Route("eliminar/{id}")]

        public IActionResult EliminarEquipo(int id)
        { 
        //Para actualizar un registro, se obtiene el registro original de la base de datos
        //al cual eliminaremos
        equipos? equipo = (from e in _equiposContext.equipos
                           where e.id_equipos == id
                           select e).FirstOrDefault();
        //Verificamos que exista el registro segun su ID
        if (equipo == null)
        return NotFound();
        //Ejecutamos la accion de elminar el registro _equiposContexto.equipos.Attach(equipo);
        _equiposContext.equipos.Remove(equipo);
        _equiposContext.SaveChanges();
        return Ok(equipo);
        }
    }

}

