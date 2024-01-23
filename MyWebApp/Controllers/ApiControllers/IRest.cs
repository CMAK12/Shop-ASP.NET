using Microsoft.AspNetCore.Mvc;

namespace MyWebApp.Controllers.ApiControllers;

interface IRest<T>
{
    [HttpGet]
    public Task<ActionResult<IEnumerable<T>>> Get();
    [HttpGet("id")]
    public Task<ActionResult<T>> Get(int id);
    [HttpPost]
    public Task<ActionResult<T>> Post(T item);
    [HttpPut]
    public Task<ActionResult<T>> Put(T item);
    [HttpDelete("id")]
    public Task<ActionResult<T>> Delete(int id);
}