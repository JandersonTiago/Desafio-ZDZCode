<template>
  <div>
    <Header />
    <h1 class="task-h1">Gerenciador de Tarefas</h1>
    
    <form class="task-form">
      <div class="form-group">
        <label for="task-title">Título da Tarefa</label>
        <input type="text" id="task-title" v-model="taskTitle" />
      </div>
      
      <div class="form-group">
        <label for="task-date">Data</label>
        <input type="date" id="task-date" v-model="taskDate" />
      </div>
      
      <div class="form-group">
        <label for="task-status">Status</label>
        <select id="task-status" v-model="taskStatus">
          <option value="Em espera">Em Espera</option>
          <option value="Em processo">Em Processo</option>
          <option value="Concluido">Concluído</option>
        </select>
      </div>
      
      <div class="form-group">
        <label for="task-responsible">Responsável</label>
        <select id="task-responsible" v-model="taskResponsible">
          <option v-for="user in users" :key="user">{{ user }}</option>
        </select>
      </div>
      
      <button type="submit" @click.prevent="cadastrarTarefa">Cadastrar</button>
    </form>
    
    <table class="task-table">
      <thead>
        <tr>
          <th>ID Tarefa</th>
          <th>Título</th>
          <th>Data</th>
          <th>Status</th>
          <th>Responsável</th>
          <th>Atualizar</th>
          <th>Excluir</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="task in tasks" :key="task.id_tarefa">
          <td>{{ task.id_tarefa }}</td>
          <td>{{ task.title }}</td>
          <td>{{ formatarData(task.date) }}</td> <!-- Formatar a data -->
          <td>{{ task.status }}</td>
          <td>{{ task.responsible }}</td>
          <td><button @click="atualizarTarefa(task)">Atualizar</button></td>
          <td><button class="delete-button" @click="excluirTarefa(task)">Excluir</button></td>
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
      taskTitle: '',
      taskDate: '',
      taskStatus: '',
      taskResponsible: '',
      tasks: [],
      nextId: 1,
      users: []
    }
  },
  methods: {
    async cadastrarTarefa() {
  try {
    const response = await this.$axios.post('http://localhost:5159/Tarefas', {
      Titulo: this.taskTitle,
      Data: this.taskDate,
      Status: this.taskStatus,
      Responsavel: this.taskResponsible
    });

    // Verifica se a resposta da API foi bem-sucedida
    if (response.status === 200) {
      const newTask = {
        id_tarefa: response.data.id_tarefa,
        title: response.data.titulo,
        date: response.data.data,
        status: response.data.status,
        responsible: response.data.responsavel
      };
      this.tasks.push(newTask);
      this.nextId++;
      this.taskTitle = '';
      this.taskDate = '';
      this.taskStatus = '';
      this.taskResponsible = '';
    } else {
      console.error('Falha ao cadastrar tarefa:', response.data);
    }
  } catch (error) {
    console.error('Erro ao cadastrar tarefa:', error);
  }
},


    atualizarTarefa(task) {
      console.log('Atualizando tarefa:', task);
    },
    excluirTarefa(task) {
      const index = this.tasks.indexOf(task);
      if (index !== -1) {
        this.tasks.splice(index, 1);
      }
    },
    async buscarUsuarios() {
      try {
        const response = await this.$axios.get('http://localhost:5159/Usuario');
        this.users = response.data.map(user => user.nome);
      } catch (error) {
        console.error('Erro ao buscar usuários:', error);
      }
    },
    formatarData(data) {
      // Formatar a data para o formato brasileiro (DD/MM/AAAA)
      if (!data) return '';
      const dataObj = new Date(data);
      const dia = dataObj.getDate().toString().padStart(2, '0');
      const mes = (dataObj.getMonth() + 1).toString().padStart(2, '0');
      const ano = dataObj.getFullYear();
      return `${dia}/${mes}/${ano}`;
    }
  },
  mounted() {
    this.buscarUsuarios();
  }
}
</script>

<style scoped>
/* Estilos específicos da página principal */
.task-form, .task-table, .task-h1 {
  font-family: Arial, Helvetica, sans-serif;
}

.task-h1 {
  margin: 20px 0;
}

.task-form {
  margin: 20px 0;
}

.form-group {
  margin-bottom: 15px;
}

.form-group label {
  display: block;
  margin-bottom: 5px;
}

.form-group input,
.form-group select {
  width: 100%;
  padding: 8px;
  box-sizing: border-box;
}

button {
  padding: 10px 15px;
  background-color: #0000FF;
  color: #fff;
  border: none;
  cursor: pointer;
  font-family: Arial, Helvetica, sans-serif;
}

button:hover {
  background-color: #000099;
}

.task-table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 20px;
}

.task-table th,
.task-table td {
  border: 1px solid #ddd;
  padding: 8px;
  text-align: left;
}

.task-table th {
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
