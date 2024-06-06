<template>
  <div>
    <Header />
    <h1>Cadastro de Usuários</h1>
    
    <form class="user-form">
      <div class="form-group">
        <label for="user-name">Nome</label>
        <input type="text" id="user-name" v-model="userName" />
      </div>
      
      <button type="submit" @click.prevent="cadastrarOuAtualizarUsuario">{{ editingUser ? 'Atualizar' : 'Cadastrar' }}</button>
    </form>
    
    <table class="user-table">
      <thead>
        <tr>
          <th>ID Usuário</th>
          <th>Nome</th>
          <th></th>
          <th></th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="user in users" :key="user.id">
          <td>{{ user.id }}</td>
          <td>{{ user.nome }}</td>
          <td><button @click="iniciarEdicao(user)">Atualizar</button></td>
          <td><button class="delete-button" @click="excluirUsuario(user)">Excluir</button></td>
        </tr>
      </tbody>
    </table>
    
    <Footer />
  </div> 
</template>

<script>
import Header from '~/components/navbar.vue'
import Footer from '~/components/footer.vue'

export default {
  components: {
    Header,
    Footer
  },
  data() {
    return {
      userName: '',
      users: [],
      editingUser: null // Para controlar o usuário que está sendo editado
    }
  },
  methods: {
    async cadastrarOuAtualizarUsuario() {
      if (this.editingUser) {
        await this.atualizarUsuario(this.editingUser);
      } else {
        await this.cadastrarUsuario();
      }
    },
    async cadastrarUsuario() {
      try {
        const response = await this.$axios({
          method: 'POST',
          url: 'http://localhost:5159/Usuario',
          data: {
            Id: this.generateUniqueId(),
            Nome: this.userName
          }
        });

        console.log('Resposta da API:', response.data);

        const newUser = {
          id: response.data.id,
          nome: response.data.nome
        };
        this.users.push(newUser);

        this.userName = '';
      } catch (error) {
        console.error('Erro ao cadastrar usuário:', error);
      }
    },
    async atualizarUsuario(user) {
      try {
        const response = await this.$axios.put(`http://localhost:5159/Usuario/${user.id}`, {
          Id: user.id,
          Nome: this.userName // Usar o nome atualizado a partir do campo de entrada
        });

        if (response.status === 200) {
          // Atualiza a lista de usuários local com os novos dados
          this.users = this.users.map(u => (u.id === user.id ? response.data : u));
          console.log('Usuário atualizado com sucesso:', response.data);
          this.editingUser = null;
          this.userName = '';
        } else {
          console.error('Falha ao atualizar usuário:', response.data);
        }
      } catch (error) {
        console.error('Erro ao atualizar usuário:', error);
      }
    },
    generateUniqueId() {
      const currentDate = new Date();
      return parseInt(currentDate.getTime() / 1000);
    },
    async buscarUsuarios() {
      try {
        const response = await this.$axios.get('http://localhost:5159/Usuario');
        this.users = response.data;

        console.log(response.data);

      } catch (error) {
        console.error('Erro ao buscar usuários:', error);
      }
    },
    async excluirUsuario(user) {
      try {
        const response = await this.$axios.delete(`http://localhost:5159/Usuario/${user.id}`);

        if (response.status === 200) {
          this.users = this.users.filter(u => u.id !== user.id);
          console.log('Usuário excluído com sucesso:', user);
        } else {
          console.error('Falha ao excluir usuário:', response.data);
        }
      } catch (error) {
        console.error('Erro ao excluir usuário:', error);
      }
    },
    iniciarEdicao(user) {
      this.userName = user.nome; // Preenche o campo de entrada com o nome do usuário a ser editado
      this.editingUser = user; // Define o usuário que está sendo editado
    }
  },
  mounted() {
    this.buscarUsuarios();
  }
}
</script>

<style scoped>
/* Estilos específicos da página principal */
h1 {
  font-family: Arial, sans-serif;
  margin: 20px 0;
}

.user-form {
  margin: 20px 0;
}

.form-group {
  margin-bottom: 15px;
}

.form-group label {
  display: block;
  margin-bottom: 5px;
}

.form-group input {
  width: 100%;
  padding: 8px;
  box-sizing: border-box;
  font-family: Arial, sans-serif;
}

button {
  padding: 10px 15px;
  background-color: #0000FF;
  color: #fff;
  border: none;
  cursor: pointer;
  font-family: Arial, sans-serif;
}

button:hover {
  background-color: #000099;
}

.user-table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 20px;
  font-family: Arial, sans-serif; 
}

.user-table th,
.user-table td {
  border: 1px solid #ddd;
  padding: 8px;
  text-align: left;
}

.user-table th {
  background-color: #f2f2f2;
}

/* Estilo para o botão de excluir */
button.delete-button {
  background-color: #DC143C; /* Tom de vermelho confortável */
}

button.delete-button:hover {
  background-color: #B22222; /* Tom de vermelho mais escuro */
}
</style>
