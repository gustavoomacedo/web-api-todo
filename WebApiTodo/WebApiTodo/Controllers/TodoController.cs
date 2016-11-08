using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApiTodo.Models;

namespace WebApiTodo.Controllers
{
    [EnableCors(origins: "*",headers: "*",methods: "*")]
    [RoutePrefix("api")]
    public class TodoController : ApiController
    {
        private bdTodoEntidades db = new bdTodoEntidades();

        // GET: api/Todo

        [Route("todo")]
        public HttpResponseMessage GetTodo()
        {
            var result = db.Todo.Include("Item").ToList();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // GET: api/Todo/1

        [Route("todo/{id}")]
        public HttpResponseMessage GetTodoById(int id)
        {
            var result = db.Todo.Where(t => t.id == id).Include("Item").ToList();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // GET: api/itens

        [Route("itens")]
        public HttpResponseMessage GetItens()
        {
            var result = db.Item.ToList();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // GET: api/todo/1/itens

        [Route("todo/{id}/itens")]
        public HttpResponseMessage GetItensByTodo(int id)
        {
            var result = db.Todo.Include("Item").Where(x => x.id == id).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // POST: api/Todo

        [HttpPost]
        [Route("todo")]
        public HttpResponseMessage PostTodo(Todo todo)
        {
            if(todo == null)
                return Request.CreateResponse(HttpStatusCode.NoContent);

            try
            {
                if(todo.feito == null)
                    todo.feito = false;
                db.Todo.Add(todo);
                db.SaveChanges();

                var result = todo;
                return Request.CreateResponse(HttpStatusCode.Created, result);
            }
            catch(Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,"Erro ao incluir produto.");
            }
        }

        // PATCH: api/Todo

        [HttpPatch]
        [Route("todo")]
        public HttpResponseMessage PatchTodo(Todo todo)
        {
            if (todo == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            try
            {
                db.Entry<Todo>(todo).State = EntityState.Modified;
                db.SaveChanges();

                var result = todo;
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Erro ao alterar produto.");
            }
        }

        // PUT: api/Todo/5

        [HttpPut]
        [Route("todo")]
        public HttpResponseMessage PutTodo(Todo todo)
        {
            if (todo == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            try
            {
                db.Entry<Todo>(todo).State = EntityState.Modified;
                db.SaveChanges();

                var result = todo;
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Erro ao alterar produto.");
            }
        }

        // DELETE: api/Todo/5

        [HttpDelete]
        [Route("todo/{id}")]
        public HttpResponseMessage DeleteTodo(int id)
        {
            if (id <= 0)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            try
            {
                db.Todo.Remove(db.Todo.Find(id));
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "Produto Excluido.");
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Falha ao excluir Todo");
            }
        }

        // GET: api/lista/1

        [Route("lista/{feito}")]
        public HttpResponseMessage GetTodoFeito(int feito)
        {
            if (feito == 1)
            {
                var result = (from t in db.Todo
                              where !(from i in db.Item
                                      where i.feito == false
                                      select i.idtodo).Contains(t.id)
                              select t
                             ).Include("Item").ToList();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                var result = (from t in db.Todo
                             from i in db.Item
                             .Where(a => a.idtodo == t.id).DefaultIfEmpty()
                             where i.feito == false
                             select t).Include("Item").ToList();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}