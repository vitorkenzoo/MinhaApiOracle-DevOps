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
            // Validação básica
            if (string.IsNullOrWhiteSpace(usuario.Nome))
                return BadRequest("O campo 'nome' é obrigatório.");
            
            if (string.IsNullOrWhiteSpace(usuario.EmailUsuario))
                return BadRequest("O campo 'emailUsuario' é obrigatório.");
            
            if (string.IsNullOrWhiteSpace(usuario.Senha))
                return BadRequest("O campo 'senha' é obrigatório.");
            
            if (string.IsNullOrWhiteSpace(usuario.Cpf))
                return BadRequest("O campo 'cpf' é obrigatório.");

            // Cria um novo usuário apenas com os dados básicos, ignorando relacionamentos
            var novoUsuario = new Usuario
            {
                Nome = usuario.Nome,
                EmailUsuario = usuario.EmailUsuario,
                Senha = usuario.Senha,
                Cpf = usuario.Cpf,
                Cadastro = usuario.Cadastro == default(DateTime) ? DateTime.Now : usuario.Cadastro,
                // IdUsuario será gerado automaticamente pelo banco (auto-incremento)
                // Certificados será null (será populado quando necessário)
            };

            _context.Usuarios.Add(novoUsuario);
            await _context.SaveChangesAsync();
            
            // Carrega o usuário criado com os relacionamentos para retornar
            var usuarioCriado = await _context.Usuarios
                .Include(u => u.Certificados)
                .FirstOrDefaultAsync(u => u.IdUsuario == novoUsuario.IdUsuario);
            
            return CreatedAtAction(nameof(GetById), new { id = novoUsuario.IdUsuario }, usuarioCriado);
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

