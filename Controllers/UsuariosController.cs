using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinhaApiOracle.Data;
using MinhaApiOracle.Models;

namespace MinhaApiOracle.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDb _context;

        public UsuariosController(AppDb context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var usuarios = await _context.Usuarios
                .Include(u => u.Certificados)
                .ToListAsync();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Certificados)
                .FirstOrDefaultAsync(u => u.IdUsuario == id);
            
            if (usuario == null) return NotFound();
            return Ok(usuario);
        }

        [HttpGet("buscar")]
        public async Task<IActionResult> BuscarPorNome([FromQuery] string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return BadRequest("O parâmetro 'nome' é obrigatório.");

            var usuarios = await _context.Usuarios
                .Include(u => u.Certificados)
                .Where(u => u.Nome.Contains(nome))
                .ToListAsync();

            if (usuarios.Count == 0) return NotFound();
            return Ok(usuarios);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Usuario usuario)
        {
            if (usuario.Cadastro == default(DateTime))
                usuario.Cadastro = DateTime.Now;

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = usuario.IdUsuario }, usuario);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Usuario usuario)
        {
            if (id != usuario.IdUsuario) return BadRequest();

            var existingUsuario = await _context.Usuarios.FindAsync(id);
            if (existingUsuario == null) return NotFound();

            existingUsuario.Nome = usuario.Nome;
            existingUsuario.EmailUsuario = usuario.EmailUsuario;
            existingUsuario.Senha = usuario.Senha;
            existingUsuario.Cpf = usuario.Cpf;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

