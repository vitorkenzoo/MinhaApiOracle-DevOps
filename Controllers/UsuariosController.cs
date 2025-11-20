using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinhaApiOracle.Data;
using MinhaApiOracle.Models;
using MinhaApiOracle.DTOs;

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
            try
            {
                var usuarios = await _context.Usuarios
                    .Include(u => u.Certificados)
                    .ToListAsync();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erro ao buscar usuários", message = ex.Message, innerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var usuario = await _context.Usuarios
                    .Include(u => u.Certificados)
                    .FirstOrDefaultAsync(u => u.IdUsuario == id);
                
                if (usuario == null) return NotFound();
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erro ao buscar usuário", message = ex.Message, innerException = ex.InnerException?.Message });
            }
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

        /// <summary>
        /// Cria um novo usuário
        /// </summary>
        /// <param name="dto">Dados do usuário (sem relacionamentos)</param>
        /// <returns>Usuário criado com ID gerado</returns>
        [HttpPost]
        public async Task<IActionResult> Create(UsuarioCreateDto dto)
        {
            try
            {
                // Validação básica
                if (string.IsNullOrWhiteSpace(dto.Nome))
                    return BadRequest("O campo 'nome' é obrigatório.");
                
                if (string.IsNullOrWhiteSpace(dto.EmailUsuario))
                    return BadRequest("O campo 'emailUsuario' é obrigatório.");
                
                if (string.IsNullOrWhiteSpace(dto.Senha))
                    return BadRequest("O campo 'senha' é obrigatório.");
                
                if (string.IsNullOrWhiteSpace(dto.Cpf))
                    return BadRequest("O campo 'cpf' é obrigatório.");

                // Cria um novo usuário apenas com os dados básicos, ignorando relacionamentos
                var novoUsuario = new Usuario
                {
                    Nome = dto.Nome,
                    EmailUsuario = dto.EmailUsuario,
                    Senha = dto.Senha,
                    Cpf = dto.Cpf,
                    Cadastro = dto.Cadastro ?? DateTime.Now,
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
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erro ao criar usuário", message = ex.Message, innerException = ex.InnerException?.Message });
            }
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

