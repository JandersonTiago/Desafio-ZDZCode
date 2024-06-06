<template>
  <div>
    <Header />
    <h1>Cadastro de Usuários</h1>
    
    <form class="user-form">
      <div class="form-group">
        <label for="user-name">Nome</label>
        <input type="text" id="user-name" v-model="userName" />
      </div>
      
      <button type="submit" @click.prevent="cadastrarUsuario">Cadastrar</button>
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
          <td><button @click="atualizarUsuario(user)">Atualizar</button></td>
          <td><button @click="excluirUsuario(user)">Excluir</button></td>
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
      lastGeneratedId: 0 // Inicializa o último ID como 0
    }
  },
  methods: {
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
</style>