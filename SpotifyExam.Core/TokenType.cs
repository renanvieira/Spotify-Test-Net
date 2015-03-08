using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyExam.Core {
	/// <summary>
	/// Enum que indica os tipos de token que será usado na hora de retornar o Access Token.
	/// </summary>
	public enum TokenType {
		/// <summary>
		/// Valor default.
		/// </summary>
		Undefined = 0,

		/// <summary>
		/// Usar o token de autorização, indica que é a primeira vez que está sendo requisitado o token.
		/// </summary>
		Authorization = 1,

		/// <summary>
		/// Usar o token de refresh, indica que o AccessToken expirou e é necessario atualiza-lo.
		/// </summary>
		Refresh = 2,

	}
}
